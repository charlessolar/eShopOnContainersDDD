import { IType, types, getEnv, getRoot } from 'mobx-state-tree';
import { JsonServiceClient } from '@servicestack/client';
import { History } from 'history';

import { Theme } from 'material-ui/styles';
import { DTOs } from './utils/eShop.dtos';
import theme from './theme';

export interface AuthenticationType {
  email: string;
  token: string;
  expires: number;
  admin: boolean;
  updateToken(email: string, token: string, expires: number): void;
  reset(): void;
}
const Authentication = types.model(
  'Authentication',
  {
    email: types.maybe(types.string),
    token: types.maybe(types.string),
    expires: types.maybe(types.number),
    admin: types.optional(types.boolean, false)
  })
  .views(self => ({
    get authenticated() {
      return self.token && self.token !== ''; // && expires > now
    }
  }))
  .actions(self => ({
    updateToken(email: string, token: string, expires: number) {
      self.email = email;
      self.token = token;
      self.expires = expires;
    },
    reset() {
      self.email = '';
      self.token = '';
      self.expires = 0;
    }
  }));

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
  alertStack: AlertStackType;
  auth: AuthenticationType;
  theme: Theme;
  history: History;

  readonly authenticated: boolean;
}
export const Store = types.model(
  'Store',
  {
    api: types.optional(ApiClient, { loading: false }),
    alertStack: types.optional(AlertStack, { stack: {} }),
    auth: types.optional(Authentication, { token: '' })
  }
)
.views(self => ({
  get authenticated() {
    return self.auth.authenticated;
  },
  get theme() {
    return getEnv(self).theme;
  },
  get history() {
    return getEnv(self).history;
  }
}));
