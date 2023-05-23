# ImpactExerciseOne
Impact Code Challenge implemented by André Lopes

My Solution explanation:

1. First, this Solution is delivered to you without compilation errors and without warnings;

2. I didn't implement a client-side. If you run this Solution, through the IISExpress, you will be directed to the Swagger UI. Where you can use the endpoints provided by this Solution's API (Basket API);

3. It's implemented the basic Unit Tests for the API. You can see it in the Unit Test project;

4. Based on the instructions pdf, that you sent me, I implemented the following operations on the server-side: 
"AddProduct"; "RemoveProduct"; "ChangeProductQuantity". In other scenario, I'd have implemented them only in client-side, and then submit them on "CreateOrder" operation;

5. As I implemented them (4.) on server-side, I added the BasketService as a Singleton Service (See: Program.cs - line 13). I chose this approach, so the "_orderLines" (see BasketService) could hold the Products being managed. When the "CreateOrder" operation returns successful, I clear the "_orderLines", for the next order (See: BasketService.cs - line 86).

