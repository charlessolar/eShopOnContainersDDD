import { AlertStack } from './components/alertStack';

import { IType, types, getEnv, getRoot } from 'mobx-state-tree';
import { JsonServiceClient } from '@servicestack/client';

import { DTOs } from './utils/eShop.dtos';

// requires https://github.com/mobxjs/mobx-state-tree/issues/117
export interface ApiClient {
  loading: boolean;
  client: JsonServiceClient;
  query<T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>): Promise<DTOs.QueryResponse<T>>;
  paged<T>(request: DTOs.IReturn<DTOs.PagedResponse<T>>): Promise<DTOs.PagedResponse<T>>;
  command<T>(request: DTOs.IReturnVoid): Promise<DTOs.CommandResponse>;
}

const ApiClient = types.model(
  'ApiClient',
  {
    loading: types.boolean,
  })
  .views(self => ({
    get client(): JsonServiceClient {
      return getEnv(this).client;
    }
  }))
  .actions(self => ({
    query<T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>) {
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

const Alert = types.model(
  'Alert',
  {
    id: types.number,
    type: types.enumeration('Type', ['Info', 'Success', 'Error', 'Warning']),
    message: types.string
  }
);

const AlertStack = types.model(
  'AlertStack',
  {
    stack: types.map(Alert)
  })
  .actions(self => ({
    add(type: 'Info' | 'Success' | 'Error' | 'Warning', message: string) {
      const id = Math.random().toString(10).split('.')[1];
      self.stack.put(Alert.create({ id, type, message }));
    },
    remove(id: string) {
      self.stack.delete(id);
    }
  }));

export const Store = types.model(
  'Store',
  {
    api: types.optional(ApiClient, {}),
    alertStack: types.optional(AlertStack, {})
  }
);
