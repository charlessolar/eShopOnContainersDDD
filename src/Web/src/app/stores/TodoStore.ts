import { observable, action, runInAction } from 'mobx';

import { inject, Client } from '../utils';
import { DTOs } from '../utils/Todo.dtos';

import { TodoModel } from 'app/models';

export class TodoStore {
  @inject(Client)
  private _client: Client;

  @observable
  public loading: boolean;
  @observable
  public todos: Array<TodoModel>;

  constructor() {
    this.loading = false;
    this.todos = new Array<TodoModel>();
  }

  @action
  public async getAllTodos() {
    this.loading = true;
    const request = new DTOs.AllTodos();

    try {
      const response = await this._client.query(request);

      this.pushTodos(response.records);
    } catch (e) {
      throw 'failed to get todos: ' + e;
    }
  }
  @action
  public async getActiveTodos() {
    this.loading = true;
    const request = new DTOs.ActiveTodos();

    try {
      const response = await this._client.query(request);

      this.pushTodos(response.records);
    } catch (e) {
      throw 'failed to get todos: ' + e;
    }
  }
  @action
  public async getCompletedTodos() {
    this.loading = true;
    const request = new DTOs.CompleteTodos();

    try {
      const response = await this._client.query(request);

      this.pushTodos(response.records);
    } catch (e) {
      throw 'failed to get todos: ' + e;
    }
  }

  @action
  private pushTodos(todos: DTOs.TodoResponse[]) {

    todos.forEach((todo) => {
      this.todos.push(new TodoModel(todo.message, !todo.active, todo.id));
    });
    this.loading = false;
  }

  @action
  public async addTodo(message: string) {
    const model = new TodoModel(message, false);
    const request = new DTOs.AddTodo();

    request.todoId = model.id;
    request.message = model.text;

    try {
      await this._client.command(request);

      runInAction("add todo", () => {
        this.todos.push(model);
      });
    } catch (e) {
      throw 'failed to save todo: ' + e;
    }
  }

  public editTodo(message: string) {
    throw 'editing disabled for now'
  }

  @action
  public async toggleTodo(id: string, completed: boolean) {
    let request: any;
    if (completed) {
      request = new DTOs.MarkTodoComplete();
      request.todoId = id;
    } else {
      request = new DTOs.MarkTodoActive();
      request.todoId = id;
    }

    try {
      await this._client.command(request);

      runInAction('change status todo', () => {
        this.todos = this.todos.map((todo) => {
          if (todo.id === id) {
            todo.completed = completed;
          }
          return todo;
        });
      });
    } catch (e) {
      throw 'failed to change todo status: ' + e;
    }
  }

  @action
  public async deleteTodo(id: string) {
    const request = new DTOs.RemoveTodo();

    request.todoId = id;

    try {
      await this._client.command(request);

      runInAction('delete todo', () => {
        this.todos = this.todos.filter((todo) => todo.id !== id);
      });
    } catch (e) {
      throw 'failed to remove todo: ' + e;
    }
  }

  @action
  public async completeAll() {

    const notcompleted = this.todos.filter(todo => !todo.completed);

    try {
      await Promise.all(notcompleted.map(async (todo) => {
        const request = new DTOs.MarkTodoComplete();
        request.todoId = todo.id;

        await this._client.command(request);

      }));

      runInAction('complete todo', () => {
        this.todos = this.todos.map((todo) => {
          if (!todo.completed) {
            todo.completed = true;
          }
          return todo;
        });
      });
    } catch (e) {
      throw 'failed to mark todos complete: ' + e;
    }
  };

  @action
  public async clearCompleted() {

    const completed = this.todos.filter(todo => todo.completed);

    try {
      await Promise.all(completed.map(async (todo) => {
        const request = new DTOs.MarkTodoActive();
        request.todoId = todo.id;

        await this._client.command(request);
      }));

      runInAction('activate todos', () => {
        this.todos = this.todos.map((todo) => {
          if (todo.completed) {
            todo.completed = false;
          }
          return todo;
        });
      });
    } catch (e) {
      throw 'failed to mark todos active: ' + e;
    }

  };
}

export default TodoStore;
