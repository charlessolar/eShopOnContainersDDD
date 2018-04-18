import { History } from 'history';
import { TodoStore } from './TodoStore';
import { RouterStore } from './RouterStore';
import { STORE_TODO, STORE_ROUTER } from 'app/constants';

export function createStores(history: History) {
  const todoStore = new TodoStore();
  const routerStore = new RouterStore(history);
  return {
    [STORE_TODO]: todoStore,
    [STORE_ROUTER]: routerStore
  };
}
