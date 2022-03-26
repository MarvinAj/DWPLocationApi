# DWPLocationApi ( DWP Technical Test - Marvin Wereko)

# Instruction
Using the language of your choice please build your own API which calls the API at https://bpdts-test-app.herokuapp.com/, and returns people who are listed as either living in London, or whose current coordinates are within 50 miles of London.

# Solution
The solution is a serverless rest api made up of 8 functions/endpoints, built using Azure functions. The actual solution has only 4 endpoints and the remaining 4 are OpenApi related. 

**Solution Endpoints**
 - UsersWithin50MilesOfLondon: [GET] {baseUrl}/city/London/users/within50milesRadius
    - Calling this endpoint will return users living within 50 miles radius of London
    
    
 - UsersWithinCityRadius: [GET] {baseUrl}/city/{city}/users
   - The function allows searching for users within a given radius of a city
    - Requires 2 parameters 
      - Path parameter : city 
      - Query parameter : radius 
      - Sample endpoint :   {baseUrl}/city/london/users?radius=50

 - GetCities: [GET] {baseUrl}/cities
   - The function returns a list of cities. 
   
 - GetCity: [GET] {baseUrl}/cities/{city}
     - The function returns a city by providing the city name as a path parameter. 

  
**OpenApi Endpoints**
  - RenderSwaggerUI: [GET] {baseUrl}/swagger/ui
    - Use the RenderSwaggerUI function to render the OpenApi documentation
  <img width="1133" alt="image" src="https://user-images.githubusercontent.com/102326285/160249829-c2fc8641-cc0d-4229-95db-646987eb3d92.png">

  - RenderOAuth2Redirect: [GET] {baseUrl}/oauth2-redirect.html
  - RenderOpenApiDocument: [GET] {baseUrl}/openapi/{version}.{extension}
  - RenderSwaggerDocument: [GET]{baseUrl}/swagger.{extension}


**Technology used:**
- Language: C#
- Azure Functions version: v4
- .Net framework: .Net6
- Unit tests framework : XUnit
- Support for OpenApi (Swagger) : OAS3

# Build & Run Solution

The source code can be built and run locally using an IDE (e.g. Visual Studio or VS code). For more information [click here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-local).

Once you click run on your IDE the below window will appear. Use the RenderSwaggerUI endpoint to access the OpenApi Swagger documentation. The function endpoints can be called from within the Swagger page. 
<img width="920" alt="image" src="https://user-images.githubusercontent.com/102326285/160251676-45b5af29-34ce-4b15-a910-d816298f943c.png">



