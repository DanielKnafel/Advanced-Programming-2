using namespace System;
using namespace System::Windows::Media;
namespace IAnomalyDetect {
	public interface class IAnomalyDetector
	{
	public:
		virtual void learn(System::String^ learnFileName);
		virtual cli::array<Tuple<System::String^, int>^>^ detect(System::String^ detectFileName);
	};
}