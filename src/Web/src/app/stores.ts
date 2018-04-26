
import { IType, types, getEnv, getRoot } from 'mobx-state-tree';
import { JsonServiceClient } from '@servicestack/client';
import { History } from 'history';

import { DTOs } from './utils/eShop.dtos';

import { CatalogStores, CatalogStoresType } from './parts/catalog/catalogModule';

// requires https://github.com/mobxjs/mobx-state-tree/issues/117
export interface ApiClientType {
  loading: boolean;
  client: JsonServiceClient;
  query<T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>): Promise<DTOs.QueryResponse<T>>;
  paged<T>(request: DTOs.IReturn<DTOs.PagedResponse<T>>): Promise<DTOs.PagedResponse<T>>;
  command<T>(request: DTOs.IReturnVoid): Promise<DTOs.CommandResponse>;
}
const ApiClient = types.model(
  'ApiClient',
  {
    loading: types.boolean
  })
  .views(self => ({
    get client(): JsonServiceClient {
      return getEnv(this).client;
    }
  }))
  .actions(self => ({
    async query<T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>) {
      self.loading = true;
      return self.client.get(request);
    },
    paged<T>(request: DTOs.IReturn<DTOs.PagedResponse<T>>) {
      return self.client.get(request);
    },
    command<T>(request: DTOs.IReturnVoid) {
      return self.client.post(request);
    }
  }));
export interface HistoryType {
  history: History;
}
const History = types.model(
  'History',
  {
  })
  .views(self => ({
    get history(): History {
      return getEnv(this).history;
    }
  }));

export interface AlertType {
  id: string;
  type: 'info' | 'warn' | 'error';
  message: string;
}
const Alert = types.model(
  'Alert',
  {
    id: types.string,
    type: types.enumeration('Type', ['info', 'warn', 'error']),
    message: types.string
  }
);

export interface AlertStackType {
  stack: Map<string, AlertType>;
  add(type: 'info' | 'warn' | 'error', message: string): void;
  remove(id: string): void;
}
const AlertStack = types.model(
  'AlertStack',
  {
    stack: types.map(Alert)
  })
  .actions(self => ({
    add(type: 'info' | 'warn' | 'error', message: string) {
      const id = Math.random().toString(10).split('.')[1];
      self.stack.put(Alert.create({ id, type, message }));
    },
    remove(id: string) {
      self.stack.delete(id);
    }
  }));

export interface StoreType {
  api: ApiClientType;
  history: HistoryType;
  alertStack: AlertStackType;
  catalog: CatalogStoresType;
}
export const Store = types.model(
  'Store',
  {
    api: types.optional(ApiClient, { loading: false }),
    history: types.optional(History, {}),
    alertStack: types.optional(AlertStack, { stack: {} }),
    catalog: types.optional(CatalogStores, {})
  }
);
