# Sympli
 Introduction
 It is a .Net Core & Angular Project
 1. front end angular page: Sympli\Sympli\ClientApp\src\app\home
 2. web api (receive request from angular page): \Sympli\Sympli\Controllers\SearchController.cs
 3. main logic & business: \Sympli\Sympli\Business\DataProcessing.cs
 4. configuration file: \Sympli\Sympli\appsettings.json
 5. independent testing framework: \Sympli\SympliUnitTest
 
 
 How to run the program
 1. download the sourcecode
 2. unzip it
 3. open Sympli.sln in visual studio 2019
 4. build the project (may take few minutes, as it needs to restore npm package)
 5. run the project, you will see the result in browser
 6. alternatively, you can update the appsettings.json and re-difine the search condition, and run the project again
 
 
 Something more
 1. as a senior full stack developer, I complete the front end component and extension 1 & 2
 2. peformance: using cache / async function to avoid waiting more times on front time
    reliability: TDD, writing independent test cases
	scalability: everything is configurable at appsettings.json
	  (1) add more (search) keywords as you like
	  (2) "website" is the rank keyword
	  (3) add more search engines as you like, 
	      "parse" is the regular express that we use to parse the search result for each search engine.
		  we can update it just in case if the search engine changes its html layout.
		  so we don't need to worry our program will break if for example google changes sth on its side.
		

		
 Improvement need to be done
 1. write log
 2. more detailed error handling (such as what if we can't get resonse from webrequest)
 3. write more test case, and inject fake data(such as Configuration) into test method