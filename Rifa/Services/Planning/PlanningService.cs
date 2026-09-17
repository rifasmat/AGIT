using System.Data;
using Microsoft.EntityFrameworkCore;
using Rifa.Data;
using Rifa.Models;

namespace Rifa.Services.Planning
{
    public class PlanningService : IPlanningService
    {
        private readonly ApplicationDbContext _db;

        public PlanningService(ApplicationDbContext db)
        {
            _db = db;
        }

        public object ProcessPlanning(ProductionDay data)
        {
            int[] jadwal = { data.Senin, data.Selasa, data.Rabu, data.Kamis, data.Jumat, data.Sabtu, data.Minggu };

            int total = jadwal.Sum();
            int jumlahHariKerja = jadwal.Count(x => x > 0);

            if (jumlahHariKerja == 0)
            {
                return new { success = false, message = "Minimal harus ada 1 hari kerja (nilai > 0)." };
            }

            int rataRata = total / jumlahHariKerja;
            int sisa = total % jumlahHariKerja;

            int[] hasil = new int[7];
            for (int i = 0; i < 7; i++)
            {
                hasil[i] = jadwal[i] > 0 ? rataRata : 0;
            }

            if (sisa > 0)
            {
                var daftarHariKerja = jadwal
                    .Select((nilai, index) => new { Index = index, Nilai = nilai })
                    .Where(x => x.Nilai > 0)
                    .OrderByDescending(x => x.Nilai)
                    .ThenBy(x => x.Index)
                    .ToList();

                for (int i = 0; i < sisa; i++)
                {
                    int posisi = daftarHariKerja[i].Index;
                    hasil[posisi] = hasil[posisi] + 1;
                }
            }

            string planningCd = GeneratePlanningCd();

            var header = new TmstPlanning
            {
                PlanningCd = planningCd,
                EmpNm = string.IsNullOrEmpty(data.EmpNm) ? "Asep" : data.EmpNm,
                Total = total,
                CreatedAt = DateTime.Now,
                IsActive = false
            };
            _db.TmstPlannings.Add(header);

            var inputUser = new TplanningUser
            {
                PlanningCd = planningCd,
                Senin = data.Senin,
                Selasa = data.Selasa,
                Rabu = data.Rabu,
                Kamis = data.Kamis,
                Jumat = data.Jumat,
                Sabtu = data.Sabtu,
                Minggu = data.Minggu
            };
            _db.TplanningUsers.Add(inputUser);

            var rekomendasi = new TplanningRecommendation
            {
                PlanningCd = planningCd,
                Senin = hasil[0],
                Selasa = hasil[1],
                Rabu = hasil[2],
                Kamis = hasil[3],
                Jumat = hasil[4],
                Sabtu = hasil[5],
                Minggu = hasil[6]
            };
            _db.TplanningRecommendations.Add(rekomendasi);

            _db.SaveChanges();

            return new
            {
                success = true,
                planningCd = planningCd,
                empNm = header.EmpNm,
                total = total,
                rencanaSenin = data.Senin,
                rencanaSelasa = data.Selasa,
                rencanaRabu = data.Rabu,
                rencanaKamis = data.Kamis,
                rencanaJumat = data.Jumat,
                rencanaSabtu = data.Sabtu,
                rencanaMinggu = data.Minggu,
                hasilSenin = hasil[0],
                hasilSelasa = hasil[1],
                hasilRabu = hasil[2],
                hasilKamis = hasil[3],
                hasilJumat = hasil[4],
                hasilSabtu = hasil[5],
                hasilMinggu = hasil[6]
            };
        }

        public List<object> GetHistory()
        {
            var data = _db.TmstPlannings
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    planningCd = x.PlanningCd,
                    empNm = x.EmpNm,
                    total = x.Total,
                    createdAt = x.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                    isActive = x.IsActive
                })
                .ToList<object>();

            return data;
        }

        public object GetDetail(string planningCd)
        {
            var header = _db.TmstPlannings.FirstOrDefault(x => x.PlanningCd == planningCd);
            var input = _db.TplanningUsers.FirstOrDefault(x => x.PlanningCd == planningCd);
            var rekomendasi = _db.TplanningRecommendations.FirstOrDefault(x => x.PlanningCd == planningCd);

            if (header == null || input == null || rekomendasi == null)
            {
                return new { success = false, message = "Data tidak ditemukan." };
            }

            return new
            {
                success = true,
                planningCd = header.PlanningCd,
                empNm = header.EmpNm,
                total = header.Total,
                isActive = header.IsActive,
                rencanaSenin = input.Senin,
                rencanaSelasa = input.Selasa,
                rencanaRabu = input.Rabu,
                rencanaKamis = input.Kamis,
                rencanaJumat = input.Jumat,
                rencanaSabtu = input.Sabtu,
                rencanaMinggu = input.Minggu,
                hasilSenin = rekomendasi.Senin,
                hasilSelasa = rekomendasi.Selasa,
                hasilRabu = rekomendasi.Rabu,
                hasilKamis = rekomendasi.Kamis,
                hasilJumat = rekomendasi.Jumat,
                hasilSabtu = rekomendasi.Sabtu,
                hasilMinggu = rekomendasi.Minggu
            };
        }

        public object UpdateStatus(string planningCd, bool isActive)
        {
            var target = _db.TmstPlannings.FirstOrDefault(x => x.PlanningCd == planningCd);

            if (target == null)
            {
                return new { success = false, message = "Data tidak ditemukan." };
            }

            if (isActive)
            {
                var semuaPlanning = _db.TmstPlannings.Where(x => x.IsActive && x.PlanningCd != planningCd).ToList();
                foreach (var item in semuaPlanning)
                {
                    item.IsActive = false;
                }
            }

            target.IsActive = isActive;
            _db.SaveChanges();

            string status = isActive ? "diaktifkan" : "dinonaktifkan";
            return new { success = true, message = "Planning " + planningCd + " berhasil " + status + "." };
        }

        private string GeneratePlanningCd()
        {
            var conn = _db.Database.GetDbConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT NEXT VALUE FOR seqplanning";
            var nextVal = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            return "PLN-" + nextVal.ToString("D4");
        }
    }
}