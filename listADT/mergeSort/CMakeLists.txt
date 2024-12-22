cmake_minimum_required(VERSION 3.27)
project("MergeSort" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include" "${PROJECT_SOURCE_DIR}/List/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

# project
add_executable(main ${PROJECT_SOURCES}/main.c ${PROJECT_SOURCES}/ioAuxiliaries.c ${PROJECT_SOURCES}/phonebook.c)
target_link_libraries(main PUBLIC list)

add_subdirectory(List)

# tests
add_executable(mainTest ${TESTS_PATH}/tests.c ${PROJECT_SOURCES}/ioAuxiliaries.c ${PROJECT_SOURCES}/phonebook.c)
target_compile_definitions(mainTest PRIVATE TESTFILE="${TESTS_PATH}/phonebook.test")
target_compile_definitions(mainTest PRIVATE CHECKFILE1="${TESTS_PATH}/1mode")
target_compile_definitions(mainTest PRIVATE CHECKFILE2="${TESTS_PATH}/2mode")

add_test(NAME "phone book sort test" COMMAND mainTest)
target_link_libraries(mainTest PUBLIC list)
