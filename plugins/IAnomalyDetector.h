		public interface AnomalyDetector
	{
	public:
		AnomalyDetector();
		void learn(System::String^ learnFileName);
		cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
		~AnomalyDetector();
	};