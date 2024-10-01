using TrainingManager.Processors;

TrainingProcessor trainingProcessor = new TrainingProcessor();
trainingProcessor.ProcessData();
trainingProcessor.ListTrainingDetails();
trainingProcessor.TraineesByTraining(new List<string>
{ 
    "Electrical Safety for Labs", 
    "X-Ray Safety", 
    "Laboratory Safety Training"
}, 2024);
trainingProcessor.TraineesWithExpiredTrainings(new DateTime(2023, 10, 1));

