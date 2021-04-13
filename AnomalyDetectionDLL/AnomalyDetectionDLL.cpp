#include "pch.h"

#include "AnomalyDetectionDLL.h"

AnomalyDetectionDLL::API::API(System::String ^filename) {
	ts = new TimeSeries((char*)
        System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(
            filename).ToPointer());
    ad = new SimpleAnomalyDetector();
    ad->setCorrelationThreshold(0.9);
    ad->learnNormal(*ts);
    std::vector<correlatedFeatures> cf = ad->getNormalModel();
    correlationNames = gcnew cli::array<System::Tuple<System::String^, System::String^>^>(cf.size());
    regLines = gcnew cli::array<System::Tuple<float, float>^>(cf.size());
    for (int i=0; i<cf.size(); i++)
    {
        correlatedFeatures c = cf[i];
        correlationNames[i] = 
            gcnew System::Tuple<System::String^, System::String^>(
                gcnew System::String(c.feature1.c_str()), gcnew System::String(c.feature2.c_str()));
        regLines[i] = gcnew System::Tuple<float, float>(c.lin_reg.a, c.lin_reg.b);
    }
}

AnomalyDetectionDLL::API::~API() {
    delete ts;
    delete ad;
}

cli::array<System::Tuple<System::String^, System::String^>^> ^ AnomalyDetectionDLL::API::getCorrelationNamesVector() {
    return correlationNames;
}

cli::array<System::Tuple<float, float>^> ^ AnomalyDetectionDLL::API::getRegLinesVector() {
    return regLines;
}