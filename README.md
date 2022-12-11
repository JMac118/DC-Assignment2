# DC-Assignment2 - P2P Application
This project was created by Sharron Foo and Joshua Macaulay for assignment two in Distributed Computing at Curtin.
<br />Peer-to-Peer Python Job Poster
<br />Following the assignment specification within the root directory of the repository, we implemented a web service
<br />to connect peers to each other as a 'tracker' and manage a scalable network of clients. Also we implemented the 
<br /> client desktop application to upload python2 code to be executed by other clients.
<br />This project was developed within the .NET Framework using C#.
# Internal Sections
Web Server (ASP.NET MVC Web Service with LocalDB)
<br />Provides the 'tracker' service to allow clients to register themselves and connect with other registered clients.
<br />Utilises a SQL database to store activity logs (which client performed which job, when job was submitted, etc).

Client Desktop Application (.NET Remoting)
<br />Utilises three separate threads for displaying GUI to user, hosting a local server to store and allow downloads of 
<br />user uploaded jobs, and for pinging each client on the P2P network for their own uploaded jobs to download.

Dynamic Network Viewer Website (ASP.NET CORE)
<br />A dynamic website that queries the web server to display all clients currently connected to the P2P swarm while
<br />also displaying each clients completed jobs. The website will auto-refresh the contents.

# Concepts
Developing a distributed computing system for processing large quantities of data in the form of 'jobs'.
<br />Managing multiple threads in a desktop application for displaying, automating and networking.
<br />Performing cloud-computing work utilising Python2 code within .NET framework by using Iron Python library.
<br />Using Base64 encoding for basic security to send data securely over a network link with potentially many clients.
<br />Verifying and authenticating data sent over the network by hashing pre-encoded data with SHA256.
<br />Implementing an SQL server database with Entity Framework object relationships.
