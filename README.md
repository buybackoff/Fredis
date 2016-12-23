Update
=======================

There are several really useful things here:
* DynamicContext for EF6 that starts per-table automatic migrations, which is very convenient while
prototyping RDMS structure and adding/changing schema. By default, only non-destructive updates are 
allowed, but this is a config setting.
* SE.Redis wrapper with automatic serialization of generic values, with JSON.NET by default.
* Redis-based distributed MPMC RedisQueue and RedisAsyncDictionary, which together allow to build any 
complex Actor topology manually without a separate Actor abstraction.

The readme below is as of 2014. I will rewrite a simpler version of Actors using the 
RedisQueue/RedisAsyncDictionary, just to keep the name of the project relevant.

Current version of Actor implementation is incomprehensible, slow, spaghetti code, written
mostly for educational purposes during my time at Hacker School (but it still works).


Ractor
=======================
<img align="right" src="https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/logo.png" />
**Ractor** (Redis Actor, also see [this](http://en.wikipedia.org/wiki/The_Diamond_Age)) is a distributed 
actor system with CLR/JVM interop and dead simple API for POCOs cache/persistence. Its API is inspired by 
F#'s [MailboxProcessor](http://msdn.microsoft.com/en-us/library/ee370357.aspx), 
[Fsharp.Actor](https://github.com/colinbull/Fsharp.Actor) and Akka(.NET) libraries. The main difference is that 
in Ractor actors are virtual and exist is Redis per se as lists of messages, while a number of ephemeral 
workers (actors' "incarnations") take messages from Redis, process them and post results back.

Benchmarks (e.g. [1](http://blog.jupo.org/2013/02/23/a-tale-of-two-queues/)) show that Redis is as performant 
as old popular message queues and even newer ones, like ZMQ. 
Existing distributed actor systems use many to many connections, a design that at the first glance 
removes a single point of failure. But a closer look reveals that such design introduces multiple points
of failure because data is stored in some random nodes and at each point in time some node acts as a central
one. If that node fails the system will have to elect another lead node, but the messages will be lost.

Ractor was build with AWS infrastructure in mind. In Amazon cloud, it is easy to create one central
Redis cluster in multiple availability zones with multiple replicas. This is the most reliable 
setup one could get, and it is available in minutes for cheap price. With this reliable central node
one could then use autoscale group and even add spot instances to the system. Random shutdowns of any 
worker nodes will not affect the system in any way. This setup gives an elastic, easy to maintain and 
(automatically) scalable to a large size system of distributed actors.


**Ractor.Persistence** is a collection of APIs for POCOs and blobs persistence and a strongly typed Redis
client based on excellent [Stackexchange.Redis](https://github.com/StackExchange/StackExchange.Redis) 
library. The typed Redis client has strong opinion about keys schema inside Redis and uses a concept of
root/owner objects to store dependent objects/collections. POCO/database persistor is implemented on top 
of Entity Framework 6 (with automatic migrations enabled for non-destructive schema changes). 
Blob persistor saves large data objects to files or S3.


Process-oriented programming
----------------------
Ractor uses [process-oriented programming](http://en.wikipedia.org/wiki/Process-oriented_programming) 
paradigm - it separates the concerns of data structures and the concurrent processes that act upon them. Data structures
reside in Redis cluster and persistent storage (RDBMS/S3/etc) which logically extend single-box memory
model to distributed scenario. Ractor actors are the concurrent processes that act upon the data.

Slides for library intro at HS (Ractor was previously known as Fredis)
----------------------

![Slide 2](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide2.JPG)
![Slide 3](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide3.JPG)
![Slide 4](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide4.JPG)
![Slide 5](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide5.JPG)
![Slide 6](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide6.JPG)
![Slide 7](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide7.JPG)
![Slide 8](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide8.JPG)
![Slide 9](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide9.JPG)
![Slide 10](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide10.JPG)
![Slide 11](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide11.JPG)
![Slide 12](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide12.JPG)
![Slide 13](https://raw.githubusercontent.com/buybackoff/Ractor.CLR/master/docs/files/img/Slides/Slide13.JPG)


Install & Usage
----------------------

	PM> Install-Package Ractor
	PM> Install-Package Ractor.Persistence
	PM> Install-Package Ractor.Persistence.AWS


Docs & test are work in progress...


License
----------------------

(c) Victor Baybekov 2016

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

This software is distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.