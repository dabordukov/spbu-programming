cmake_minimum_required(VERSION 3.27)
project("Queue" LANGUAGES C)

include_directories("${PROJECT_SOURCE_DIR}/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")

enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

add_executable(testQueue ${TESTS_PATH}/tests.c)

add_library(queue STATIC ${PROJECT_SOURCES}/queue.c ${PROJECT_SOURCES}/tokens.c ${PROJECT_SOURCES}/memoryAuxiliaries.c)
target_link_libraries(testQueue queue)

add_test(NAME "Queue Test" COMMAND testQueue)
