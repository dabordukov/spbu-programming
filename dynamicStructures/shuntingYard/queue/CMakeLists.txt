cmake_minimum_required(VERSION 3.27)
project("Queue" LANGUAGES C)
enable_testing()


set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

add_executable(testQueue tests.c)

add_library(queue STATIC queue.c ../tokens.c ../memoryAuxiliaries.c)
target_link_libraries(testQueue queue)

add_test(NAME "Queue Test" COMMAND testQueue)
