#pragma once
#include "timeseries.h"
#include "SimpleAnomalyDetector.h"
#include "IAnomalyDetector.h"
using namespace System;
using namespace System::Windows::Media;
namespace AnomalyDetect {
	public ref class AnomalyDetector : public IAnomalyDetect::IAnomalyDetector
	{
		SimpleAnomalyDetector* ad;
	public:
		AnomalyDetector();
		virtual void learn(System::String^ learnFileName);
		virtual cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
		~AnomalyDetector();
	};
}
