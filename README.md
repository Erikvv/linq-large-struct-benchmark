Linq Large Struct Benchmark
===========================

This compares the performance of Linq's .Where() on readonly structs vs classes in C# 7.3.

Filtering 10 million elements to 5 million (random, no branch prediction possible):

* 68 byte struct:  1,5 seconds
* 4 byte struct: 0,3 seconds
* 68 byte class: 2,1 seconds
* 4 byte class: 1,1 seconds

Filtering 10 million elements to ~5 (branch prediction kicks in):

* 68 byte struct: ~0,65 seconds
* 4 byte struct: ~0,24 seconds
* 68 byte class: ~0,69 seconds
* 4 byte class: ~0,30 seconds

Filtering 10 million elements to ~9.999.995 (still branch prediction but more copying):

* 68 byte struct: ~1,4 seconds
* 4 byte struct: ~0,29 seconds
* 68 byte class: ~2,4 seconds
* 4 byte class: ~1,1 seconds

Conclusion
==========

Structs are faster in all cases, but as expected structs are even better when they're small.
