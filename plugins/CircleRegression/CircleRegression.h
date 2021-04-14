#pragma once
#include "HybridAnomalyDetector.h"
using namespace System;

namespace AnomalyDetect {
	public ref class AnomalyDetector
	{
		HybridAnomalyDetector* ad;
	public:
		AnomalyDetector();
		void learn(System::String^ learnFileName);
		cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
		~AnomalyDetector();
	};
}
