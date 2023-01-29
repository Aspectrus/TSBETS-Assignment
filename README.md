# Changes

* Replaced LogLine storage from list to ConcurrentQueue for faster delete operations, using concurrent for thread safety (for even faster performance tasks/threadpool could be used for each write call).
* Async Log can be injected with different types of log writers, in this case its injected with FileLogWriter.
* Moved File Log Specific Logic to FileLogWriter class.
* Added statements to close queue after stopping, so the consumer cannot add no more messages.
* Added child class FileLogiLine to LogLine with overided formmated text output.
* Added class FileLogWriter what encapsulates StreamWriter and uses LogLine objects for writing.
* Added IDisposable to FileLogWriter and AsyncLog to close file after changing midnight or AsyncLog class is not needed.
# Unit tests

* Each unit test method creates its own isolated unique folder for file testing, folder is deleted in the end of the method.
* For unit test, on midnight change, added TimeProvider Ambient asbstraction so utc.now method can return specific dates(TimeProvider is now needs to be used instead of
 DateTime instance for retrieving current date). Alternatively could have injected a time abstraction to the AsyncLog constructor

# Notes/Issues

* When creating new files after midnight logline date is not taken into account, computer date could mismatch log timestamp. It is unspecified how to deal with this case.