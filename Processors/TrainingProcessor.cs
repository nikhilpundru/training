
using System.Collections;
using System.Text.Json;
using TrainingManager.Models;

namespace TrainingManager.Processors
{
    public class TrainingProcessor
    {
        public List<Trainee> trainees = new List<Trainee>();
        public void ProcessData()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "Inputs", "trainings.txt");

            try
            {
                string jsonString = File.ReadAllText(filePath);
                if (jsonString != null)
                {
                    this.trainees = JsonSerializer.Deserialize<List<Trainee>>(jsonString) ?? new List<Trainee>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ListTrainingDetails()
        {
            Dictionary<string, int> trainingCompletionCount = new Dictionary<string, int>();
            foreach (var trainee in trainees)
            {
                var mostRecentTrainings = trainee.Completions
                    .GroupBy(t => t.Name)
                    .Select(g => g.OrderByDescending(t => t.CompletionDate).First());
                foreach (var training in mostRecentTrainings)
                {
                    if (trainingCompletionCount.ContainsKey(training.Name))
                    {
                        trainingCompletionCount[training.Name]++;
                    }
                    else
                    {
                        trainingCompletionCount[training.Name] = 1;
                    }
                }
            }
            this.WriteToJSON("trainingCompletionCounts.json", trainingCompletionCount);
        }

        public void TraineesByTraining(List<string> trainings, int fiscalYear)
        {
            DateTime fiscalYearStart = new DateTime(fiscalYear-1, 7, 1);
            DateTime fiscalYearEnd = new DateTime(fiscalYear, 6, 30);
            Dictionary<string, List<string>> traineesOfTraining = new Dictionary<string, List<string>>();
            foreach (var trainingName in trainings)
            {
                traineesOfTraining[trainingName] = new List<string>();
            }

            foreach (var trainee in this.trainees)
            {
                var recentTrainings = trainee.Completions
                    .Where(t => trainings.Contains(t.Name) &&
                                t.CompletionDate >= fiscalYearStart &&
                                t.CompletionDate <= fiscalYearEnd)
                    .GroupBy(t => t.Name)
                    .Select(g => g.OrderByDescending(t => t.CompletionDate).FirstOrDefault())
                    .Where(t => t != null);

                foreach (var training in recentTrainings)
                {
                    if (training != null && !traineesOfTraining[training.Name].Contains(trainee.Name))
                    {
                        traineesOfTraining[training.Name].Add(trainee.Name);
                    }
                }
            }
            this.WriteToJSON("traineesByTraining.json", traineesOfTraining);
        }

        public void TraineesWithExpiredTrainings(DateTime expirationDate)
        {
            var expiredTrainings = new List<ExpiredTrainingsOutput>();

            foreach (var trainee in trainees)
            {
                var recentTrainings = trainee.Completions
                    .GroupBy(t => t.Name)
                    .Select(g => g.OrderByDescending(t => t.CompletionDate).FirstOrDefault())
                    .Where(t => t != null &&
                                (t.ExpiresOn < expirationDate.AddDays(1) ||
                                 (t.ExpiresOn >= expirationDate && t.ExpiresOn <= expirationDate.AddMonths(1))));

                foreach (var training in recentTrainings)
                {
                    ExpiredTrainingsOutput traineeDetails = new ExpiredTrainingsOutput { 
                        Name = trainee.Name,
                        Completions = new List<ExpiredTraining>()
                    };

                    if (training != null)
                    {
                        string status = training.ExpiresOn < expirationDate.AddDays(1) ? "Expired" : "Expires Soon";
                        traineeDetails.Completions.Add(new ExpiredTraining
                        {
                            Name = training.Name,
                            TrainingStatus = status,
                            Timestamp = training.CompletionDate?.ToString("M/d/yyyy") ?? null,
                            Expires = training.ExpiresOn?.ToString("M/d/yyyy") ?? null
                        });
                    }
                    expiredTrainings.Add(traineeDetails);
                }
            }
            this.WriteToJSON("traineesWithExpirations.json", expiredTrainings);
        }

        public void WriteToJSON(string fileName, IEnumerable fileContent)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Directory.CreateDirectory("Outputs");
            string newJsonString = JsonSerializer.Serialize(fileContent, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(currentDirectory, "Outputs", fileName), newJsonString);
        }
    }
}
