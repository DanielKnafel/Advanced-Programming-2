# Advanced-Programming-2 - Flight Inspection App

## Introduction
In this project we connect to the FlightGear Simulator (fgfs) and provide it with a set of pre-recorded measurements of an airplanes instruments to simulate the original flight. We also show a veriety of the airplane's instruments such as altitude, airspeed, pitch-roll-yaw degrees, aileron, elevator and rudder in real-time on the screen.

In this project we're using:
1) **.NET Framework** to create a GUI Application.
2) **MVVM** architectural pattern in a multi-threaded environment.
3) **TCP** protocol for communication with the FlightGear Simulator.

The program takes a CSV file containing the recorded flight's measurements.

We created an on-screen control unit, which includes: 
1. **Joystick** that recreates the movement of the on-plane joystick.
2. **Dashboard** to display the numerical data sent from the airplane.
3. **Graph Control** to display flight's correlated features and the detected anomalies.
4. **Graph Regression** displaying all points in comparsion with a pre-learned line regression. 
5. **Video Control** to play the recorded flight making the simulator come to live.

We use our project from the previous semester written in C++ in order to load it to this project so that we'll use
all the functionality for learning the anomalies at any desired flight. We wrapped this C++ code with a dll (Dynamic Link Library) 
and loaded it to our current project.

The files we load to simulator are:
1. One file is a **normal** and regular flight containing all necessary flight's details. We're using this file and with our DLL we learn how the correct flight supposed to be.
2. Second file is an **abnormal** flight which means a flight with anomaly, flawed flight. This time we supposed to detect the anomalies, based on what we learned before.

## Explanation of folders and main files structure
In branch master we have these files:

![folders](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/folders.png)

### Explanation of each folder
#### AnomalyDetectionDLL

![AnomalyDetectionDLL](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/anommalyDetectionDLL.png)

Contains the cpp files from the first semester responsible for learning and detecting anomalies.
#### Ex1

![Ex1](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/Ex1.png)

Explanation follows after files' description.

***`CorrelatedFeaturesCalc.cs`*** - By providing a file name, the class method finds all correlated features.
Then returns the correlated features for the specific feature name provided and also returns the appropriate linear regression line.
. 

***`DataFileReader.cs`*** - The DataFileReader object receives the CSV file to be read, and reads it line-by-line. It notifies all of its listeners about the progress, allowing them to ask it for the new information at every step.

***`Line.cs`*** - Line represented by two points.

***`MainViewModel.cs`*** - The all ViewModels, GraphControl and MainWindow are listening to **MainViewModel** according to the MVVM architecture, and the MainViewModel is listening in its turn to the DataFileReader. 

***`MainWindow.xaml.cs`*** - The main view that contains all the controls of the project and displays them.

***`playback_small.xml`*** - The xml for the flight simulator.

##### client

![client](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/client.png)

The client that connects to the flight simulator server and is listening to the DataFileReader.

##### images

![images](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/images.png)

Images for the controls.

##### controls

![controls](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls.png)

Contains all the Controls of the project.

Explanation of the controls:

***`DashBoard`***

![DashBoard](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/dashboard.png)

Dashboard main role is to display the numerical data sent from the airplane. It contains the view and viewModel. The viewModel is listening to MainViewModel. It also implements ViewModel abstract class.

In addition there's a class `KnotsToAnglesConverter` that converts the knots to angles using scaling calculations. 

***`GraphControl`***

![GraphControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphcontrol.png)

Graph Control to display flight's correlated features and the detected anomalies. Contains view that is listening to the MainViewModel.

***`GraphReg`***

![GraphReg](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphreg.png)

Graph Regression is displaying all the points in comparsion with a pre-learned linear line regression. It contains view and viewModel. The viewModel is listening to the MainViewModel. It also implents ViewModel abstract class.


***`Joystick`***

![Joystick](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/joystik.png)

Joystick that uses the ailerons, elevator and rudder to control the flight. Contains view, viewModel and model. The viewModel is listening to MainViewModel and implents ViewModel abstract class.


***`VideoControl`***

![VideoControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/videocontrol.png)

Video Control main role is to play the recorded flight making the simulator come to live. It contains view, viewModel and model. The viewModel is listening to MainViewModel. It also implents ViewModel abstract class.

***`ViewModel.cs` ***

The abstract class for the ViewModels. It containts the DataFileReader and implents INotifyPropertyChanged.
#### UML

Contains images of UMLs.

#### images
Contains images to display in README.md file.

#### plugins

![plugins](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/plugins.png)

Plugins to load into the project as DLLs and the flight csv file to be inspected.

## Preinstall for developers

## Quickstart: Installations Instructions And Running
System requirements: 
1) .NET Framework
2) Microsoft.SDK.Expression.Blend

Downloading and Installing Flight Gear: 
https://www.flightgear.org/download/
(depends on operating system it will run on).

## Links

In our project we used MVVM architecture in order to make everything work.

### MVVM

![MVVM](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/MVVM.png)

The MVVM is divided to three parts:
View: responsiple to display the data to the user.
ViewModel: responsible to connect between the View and the Model.
Model: The algoritim to commit.
We are using Data Binding Between the View and the ViewModel, if proprety has changed, so as well it will be changed to the value that is binding to this proprety.
The View send Commands to the ViewModel that passes those commands to the Model. View gets notfications from the ViewModel if he listens. 
The ViewModel also get notfications but from the model for what he listens.

### Views

![Views](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Views.png)

The **views** - GraphReg, Joystick, VideoControl, Dashboard and MainWindow passing commands to their **viewModels**,
and get notified accordingly from the viewModel. That way we bind user interface objects to viewModel, 
so the connection between view and viewModel is bidirectional.


### ViewModels

![ViewModels](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/ViewModels.png)

VideoControlViewModel, DashBoardViewModel, JostickViewModel and GraphRegViewModel extends the abstract class ViewModel that Implents the InotifyPropretyChanged.
The ViewModel contains the MainViewModel, and the mentioned ViewModels above contains it as well because they extend ViewModle.
Each ViewModel Above listens according to the MVVM architecture to the MainViewModel. 
The MainView Model listens to the DataFileReader(Model) also according to the MVVM architecture.

### Models

![Models](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Models.png)

Each Model has it's own algoritm that works according to its specific logic.
Client - connects to the flight gear simulator and sends data according to the DataFileReader its listening to by MVVM architecture.
JoystickModel -  responsible for the Joystick algorithm.
DataFileReader - the main algorithm of our project, manages almost everything.

### Other

![Other](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Other.png)

### MVVM in our project

![MVVMDIAG](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/MVVMDIAG.png)

Explanation for the diagram attached above:
1) The GraphReg, Joystick, ViewControl and Dashboard **views** are all work with their **ViewModels**.
2) The MainWindow and GraphControl **views** plus GraphReg, Joystick, ViewControl and Dashboard **ViewModles** are all works with the MainViewModel.
3) The joystick also has his own **Model**.
4) The MainViewModel and the client work together with the DataFileReader.
5) The flow of the data is happening that way: views -> ViewModels -> MainViewModel -> DataFileReader(Model) (with commands and binding), Client -> DataFileReader(Model).
in the opposite way, they get notified from DataFileReader(Model) -> MainViewModel and Client -> ViewModels -> Views.

## Video link

![Models](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Models.png)
