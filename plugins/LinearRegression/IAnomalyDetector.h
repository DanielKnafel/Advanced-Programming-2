using namespace System;

public interface class AnomalyDetector
{
public:
	void learn(System::String^ learnFileName);
	cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
};