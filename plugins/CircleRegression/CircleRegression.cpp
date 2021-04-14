#include "pch.h"
#include "CircleRegression.h"

#define CORRELATION_THRESHOLD 0.9

AnomalyDetect::AnomalyDetector::AnomalyDetector() {
	this->ad = new HybridAnomalyDetector();
}

void AnomalyDetect::AnomalyDetector::learn(System::String^ learnFileName) {
	TimeSeries ts((char*)System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(
		learnFileName).ToPointer());
	this->ad->learnNormal(ts);
	this->ad->setCorrelationThreshold(CORRELATION_THRESHOLD);
}

cli::array<Tuple<System::String^, int>^>^ AnomalyDetect::AnomalyDetector::detect(System::String^ detectFileName) {
	TimeSeries ts((char*)System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(
		detectFileName).ToPointer());
	std::vector<AnomalyReport> ar = this->ad->detect(ts);
	cli::array<Tuple<System::String^, int>^>^ anomalies =
		gcnew cli::array<Tuple<System::String^, int>^>(ar.size());

	for (int i = 0; i < ar.size(); i++)
	{
		AnomalyReport a = ar[i];
		System::String^ names = gcnew System::String(a.description.c_str());
		anomalies[i] = gcnew System::Tuple<System::String^, int>(names, a.timeStep);
	}
	return anomalies;
}

AnomalyDetect::AnomalyDetector::~AnomalyDetector() {
	delete this->ad;
}