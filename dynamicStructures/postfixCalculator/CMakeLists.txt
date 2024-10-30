cmake_minimum_required(VERSION 3.27)
project(PostfixCalculator LANGUAGES C)
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES "main.c" "ioAuxiliaries.c" "postfixCalculator.c" "postfixCalculatorTokens.c")
set(TEST_SOURCES "tests.c" "postfixCalculator.c" "postfixCalculatorTokens.c")

add_executable(main ${PROJECT_SOURCES})
add_executable(testPostfixCalc ${TEST_SOURCES})

add_subdirectory(stackOnPointers)
target_link_libraries(main PUBLIC stackOnPointers)
target_link_libraries(testPostfixCalc PUBLIC stackOnPointers)

add_test(NAME "Postfix Calculator Test" COMMAND testPostfixCalc)
