# Advanced-Programming-2 - Flight Inspection App

## Introduction

In our project we connect to flight gear simulator.
We simulate a flight using a giving flight's csv files with necessary details regarding airplane's state
at any given second from the beginning - of airplane's taking off, till the end - of its landing.

In this project we're using:
1) .NET Framework to create a GUI App for FlightGear.
2) MVVM architectural pattern in a multi-threaded environment.
3) TCP protocol for Client to send, receive and parse data from FlightGear.

The csv files used in this project include details the pilot or the autopilot are using such as:
Airplane's altitude, airspeed, pitch-roll-yaw degrees, joystick's details like aileron, elevator and rudder and so on.

We created fligth's controls pack which includes: 
1. Joystick that uses the ailerons, elevator and rudder to control the flight.
2. Dashboard to display the numerical data sent from the airplane.
3. Graph Control to display flight's correlated features and the detected anomalies.
4. Graph Regression displaying all points in comparsion with a pre-learned line regression. 
5. Video Control to play the recorded flight making the simulator come to live.
We use our project from the previous semester written in C++ in order to load it to this project so that we'll use
all the functionality for learning the anomalies at any desired flight. We wrapped this C++ code with a dll (Dynamic Link Library) 
and loaded it to our current project.

The files we load to simulator are:

1. One file is a normal and regular flight containing all necessary flight's details using this file and our dll we learn how the correct flight supposed to be.
2. Second file is an abnormal flight which means a flight with anomaly, flawed flight. This time we supposed to detect the anomalies, based on what we learned before.

## Explanation of folders and main files structure
In the master we have the following files:

![folders](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/folders.png)

### Explanation of each folder
#### AnomalyDetectionDLL

![AnomalyDetectionDLL](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/anommalyDetectionDLL.png)

Contains the cpp files from the first semester for learning and detecting anomalies.
#### Ex1

![Ex1](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/Ex1.png)

Explanation follows files description.

`CorrelatedFeaturesCalc.cs` - By providing a file name, the class method finds all correlated features.
Then returns the correlated features for a specific feature name provided and also returns the appropriate linear regression line.
. 

`DataFileReader.cs` - by given a file name, reader the file and connect to the client. the client and the MainViewModel listens to him by getting events. 
it is the main part of the project that manages almost everthing(model).

`Line.cs` - represents a line by two points.

`MainViewModel.cs` - all the ViewModels, GraphControl, MainWindow listens to his according to the architecture of MVVM. listens to the DataFileReader. 

`MainWindow.xaml.cs` - The main view that contains all the controls of the project and displays them.

`playback_small.xml` - xml for the flight simulator.

##### client

![client](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/client.png)

The client that connects to the flight simulator server and listens to the DataFileReader.

##### images

![images](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/images.png)

Images for the controls.

##### controls

![controls](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls.png)

Contains all the Controls of the project.

Explanation of the controls:

`DashBoard`

![DashBoard](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/dashboard.png)

Dashboard to display the numerical data sent from the airplane. Contains view and viewModel. the viewModel listens to MainViewModel. implents ViewModel abstract class.

Also has a class `KnotsToAnglesConverter` that converts from knots to angles. 

`GraphControl`

![GraphControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphcontrol.png)

Graph Control to display flight's correlated features and the detected anomalies. Contains view that listens to MainViewModel.

`GraphReg`

![GraphReg](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/graphreg.png)

Graph Regression displaying all points in comparsion with a pre-learned line regression. Contains view and viewModel. the viewModel listens to MainViewModel. implents ViewModel abstract class.


`Joystick`

![Joystick](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/joystik.png)

Joystick that uses the ailerons, elevator and rudder to control the flight. Contains view, viewModel and model. the viewModel listens to MainViewModel. implents ViewModel abstract class.


`VideoControl`

![VideoControl](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/controls/videocontrol.png)

Video Control to play the recorded flight making the simulator come to live. Contains view, viewModel and model. the viewModel listens to MainViewModel. implents ViewModel abstract class.

`ViewModel.cs` 

abstract class for the ViewModels. containts the DataFileReader and implents INotifyPropertyChanged.
#### UML

Contains images of UMLs.

#### images
Contains images to display in README.md file.

#### plugins

![plugins](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/images/plugins.png)

Plugins to load into the project as dll. two plugins:

1. One file is a normal and regular flight containing all necessary flight's details using this file and our dll we learn how the correct flight supposed to be.

2. Second file is an abnormal flight which means a flight with anomaly, flawed flight. This time we supposed to detect the anomalies.

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

Our views - GraphReg, Joystick, VideoControl, Dashboard and MainWindow passing commands to their view model,
and get notified accordingly from the view model. That way bind user interface objects to view model, 
so the connection between view and view model is bidirectional


### ViewModels

![ViewModels](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/ViewModels.png)

VideoControlViewModel, DashBoardViewModel, JostickViewModel and GraphRegViewModel extends the abstract class ViewModel that Implents the InotifyPropretyChanged.
The ViewModel contains the MainViewModel, and the mentioned ViewModels above contains it as well because they extend ViewModle.
Each ViewModel Above listens according to the MVVM architecture to the MainViewModel. 
The MainView Model listens to the DataFileReader(Model) also according to the MVVM architecture.

### Models

![Models](https://raw.githubusercontent.com/DanielKnafel/Advanced-Programming-2/master/UML/Models.png)

Each Model has it's algoritm that it works according to him.
Client - connects to the flight gear simulator and send it's data according to the DataFileReader he's listening to by MVVM architecture.
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
