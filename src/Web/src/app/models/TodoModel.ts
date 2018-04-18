import { observable } from 'mobx';
import uuid from 'uuid/v4';

export class TodoModel {
  readonly id: string;
  @observable public text: string;
  @observable public completed: boolean;

  constructor(text: string, completed: boolean = false, id?: string) {
    this.id = id || uuid();
    this.text = text;
    this.completed = completed;
  }
}

export default TodoModel;
