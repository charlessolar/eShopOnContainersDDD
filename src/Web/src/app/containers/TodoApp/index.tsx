import * as React from 'react';
import * as style from './style.css';
import { inject, observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { Header } from 'app/components/Header';
import { TodoList } from 'app/components/TodoList';
import { Footer } from 'app/components/Footer';
import { TodoStore, RouterStore } from 'app/stores';
import {
  STORE_TODO,
  STORE_ROUTER,
  TODO_FILTER_LOCATION_HASH,
  TodoFilter
} from 'app/constants';

export interface TodoAppProps extends RouteComponentProps<any> {
  /** MobX Stores will be injected via @inject() **/
  // [STORE_ROUTER]: RouterStore;
  // [STOURE_TODO]: TodoStore;
}

export interface TodoAppState {
  filter: TodoFilter;
}

@inject(STORE_TODO, STORE_ROUTER)
@observer
export class TodoApp extends React.Component<TodoAppProps, TodoAppState> {
  constructor(props: TodoAppProps, context: any) {
    super(props, context);
    this.state = { filter: TodoFilter.ALL };
  }

  componentWillMount() {
    const todoStore = this.props[STORE_TODO] as TodoStore;
    this.checkLocationChange();
    todoStore.getAllTodos();
  }


  componentWillReceiveProps(nextProps: TodoAppProps, nextContext: any) {
    this.checkLocationChange();
  }

  checkLocationChange() {
    const router = this.props[STORE_ROUTER] as RouterStore;
    const filter = Object.keys(TODO_FILTER_LOCATION_HASH)
      .map((key) => Number(key) as TodoFilter)
      .find(
        (filter) => TODO_FILTER_LOCATION_HASH[filter] === router.location.hash
      );
    this.setState({ filter });
  }

  private handleFilter = (filter: TodoFilter) => {
    const router = this.props[STORE_ROUTER] as RouterStore;
    const currentHash = router.location.hash;
    const nextHash = TODO_FILTER_LOCATION_HASH[filter];
    if (currentHash !== nextHash) {
      router.replace(nextHash);
    }
  };

  render() {
    const todoStore = this.props[STORE_TODO] as TodoStore;
    const { children } = this.props;
    const { filter } = this.state;

    if(todoStore.loading) {
      return (<h2>Loading...</h2>);
    }

    return (
      <div className={style.normal}>
        <Header addTodo={(msg) => todoStore.addTodo(msg)} />
        <TodoList
          todos={todoStore.todos}
          completeAll={() => todoStore.completeAll()}
          deleteTodo={(id) => todoStore.deleteTodo(id)}
          editTodo={(msg) => todoStore.editTodo(msg)}
          toggleTodo={(id, completed) => todoStore.toggleTodo(id, completed)}
        />
        <Footer
          filter={filter}
          activeCount={-1}
          completedCount={-1}
          onClearCompleted={todoStore.clearCompleted}
          onChangeFilter={this.handleFilter}
        />
        {children}
      </div>
    );
  }
}
