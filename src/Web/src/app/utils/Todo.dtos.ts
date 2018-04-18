/* tslint:disable */
/* Options:
Date: 2018-04-09 21:21:53
Version: 5.02
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://10.0.0.201:8080

//GlobalNamespace: DTOs
//MakePropertiesOptional: True
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion:
//AddDescriptionAsComments: True
//IncludeTypes:
//ExcludeTypes:
//DefaultImports:
*/


export module DTOs
{

    export interface IReturn<T>
    {
        createResponse() : T;
    }

    export interface IReturnVoid
    {
        createResponse() : void;
    }

    export class TodoResponse
    {
        id: string;
        active: boolean;
        message: string;
    }

    export class Paged<TResponse>
    {
        elapsedMs: number;
        total: number;
        records: TResponse[];
    }

    /**
    * Todo
    */
    // @Route("/todo/all", "GET")
    // @Api(Description="Todo")
    export class AllTodos implements IReturn<Paged<TodoResponse>>
    {
        createResponse() { return new Paged<TodoResponse>(); }
        getTypeName() { return "AllTodos"; }
    }

    /**
    * Todo
    */
    // @Route("/todo/active", "GET")
    // @Api(Description="Todo")
    export class ActiveTodos implements IReturn<Paged<TodoResponse>>
    {
        createResponse() { return new Paged<TodoResponse>(); }
        getTypeName() { return "ActiveTodos"; }
    }

    /**
    * Todo
    */
    // @Route("/todo/complete", "GET")
    // @Api(Description="Todo")
    export class CompleteTodos implements IReturn<Paged<TodoResponse>>
    {
        createResponse() { return new Paged<TodoResponse>(); }
        getTypeName() { return "CompleteTodos"; }
    }

    /**
    * Todo
    */
    // @Route("/todo", "POST")
    // @Api(Description="Todo")
    export class AddTodo implements IReturnVoid
    {
        todoId: string;
        message: string;
        createResponse() {}
        getTypeName() { return "AddTodo"; }
    }

    /**
    * Todo
    */
    // @Route("/todo/{TodoId}", "DELETE")
    // @Api(Description="Todo")
    export class RemoveTodo implements IReturnVoid
    {
        todoId: string;
        createResponse() {}
        getTypeName() { return "RemoveTodo"; }
    }

    /**
    * Todo
    */
    // @Route("/todo/{TodoId}/active", "POST")
    // @Api(Description="Todo")
    export class MarkTodoActive implements IReturnVoid
    {
        todoId: string;
        createResponse() {}
        getTypeName() { return "MarkTodoActive"; }
    }

    /**
    * Todo
    */
    // @Route("/todo/{TodoId}/complete", "POST")
    // @Api(Description="Todo")
    export class MarkTodoComplete implements IReturnVoid
    {
        todoId: string;
        createResponse() {}
        getTypeName() { return "MarkTodoComplete"; }
    }

}
