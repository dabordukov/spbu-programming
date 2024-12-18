cmake_minimum_required(VERSION 3.27)
project("Hashtable" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -g")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/hashtable.c ${SOURCES_PATH}/ioAuxiliaries.c)

add_subdirectory(List)

# project
add_library(hashtable STATIC ${PROJECT_SOURCES})
target_link_libraries(hashtable PRIVATE list)

# tests
add_executable(hashtableTest ${TESTS_PATH}/tests.c ${PROJECT_SOURCES})
target_link_libraries(hashtableTest PRIVATE hashtable)
target_compile_definitions(hashtableTest PRIVATE HASHTABLE_WORD_COUNTER_TEST_FILENAME="${TESTS_PATH}/words")
target_compile_definitions(hashtableTest PRIVATE HASHTABLE_WORD_COUNTER_TEST_RESULT_FILENAME="${TESTS_PATH}/testResult")

add_test(NAME "Hashtable test" COMMAND hashtableTest)
