import { IType, types, getEnv, getRoot, flow, applySnapshot, onSnapshot, addDisposer } from 'mobx-state-tree';
import { JsonServiceClient } from '@servicestack/client';
import { History } from 'history';
import Debug from 'debug';

import { Theme } from 'material-ui/styles';
import { DTOs } from './utils/eShop.dtos';
import theme from './theme';

const debug = new Debug('stores');

export interface ConfigurationStatusType {
  isSetup: boolean;
  setup: () => void;
}
export const ConfigurationStatusModel = types
  .model({
    isSetup: types.boolean,
  })
  .actions(self => ({
    setup() {
      self.isSetup = true;
    }
  }));

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
    },
    afterCreate() {
      const authStorage = localStorage.getItem('auth');
      applySnapshot(self, authStorage ? JSON.parse(authStorage) : {});

      const disposer = onSnapshot(self, state => {
        localStorage.setItem('auth', JSON.stringify(state));
      });
      addDisposer(self, disposer);
    }
  }));

// requires https://github.com/mobxjs/mobx-state-tree/issues/117
export interface ApiClientType {
  loading: boolean;
  query: <T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>) => Promise<DTOs.QueryResponse<T>>;
  paged: <T>(request: DTOs.IReturn<DTOs.PagedResponse<T>>) => Promise<DTOs.PagedResponse<T>>;
  command: <T>(request: DTOs.IReturn<DTOs.CommandResponse>) => Promise<DTOs.CommandResponse>;
}
const ApiClient = types.model(
  'ApiClient',
  {
    loading: types.optional(types.boolean, false)
  })
  .actions(self => {
    const query = flow(function*<T>(request: DTOs.IReturn<DTOs.QueryResponse<T>>) {
      const client = getEnv(self).client as JsonServiceClient;
      self.loading = true;
      try {
        const response = yield client.get(request);
        self.loading = false;
        return response;
      } catch (error) {
        debug('failed to execute query: ', error);
        self.loading = false;
      }
    });
    const paged = flow(function*<T>(request: DTOs.IReturn<DTOs.PagedResponse<T>>) {
      const client = getEnv(self).client as JsonServiceClient;
      try {
        const response = yield client.get(request);
        self.loading = false;
        return response;
      } catch (error) {
        debug('failed to execute paged query: ', error);
        self.loading = false;
      }
    });
    const command = flow(function*<T>(request: DTOs.IReturn<DTOs.CommandResponse>) {
      const client = getEnv(self).client as JsonServiceClient;
      try {
        const response = yield client.post(request);
        self.loading = false;
        return response;
      } catch (error) {
        debug('failed to execute command: ', error);
        self.loading = false;
      }
    });

    return { query, paged, command };
  });

export interface AlertType {
  id: string;
  type: 'info' | 'warn' | 'error';
  message: string;
}
const Alert = types.model(
  'Alert',
  {
    id: types.identifier(types.string),
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
      self.stack.put({ id, type, message });
    },
    remove(id: string) {
      self.stack.delete(id);
    }
  }));

export interface StoreType {
  api: ApiClientType;
  alertStack: AlertStackType;
  auth: AuthenticationType;
  status: ConfigurationStatusType;
  theme: Theme;
  history: History;

  readonly authenticated: boolean;
  load: () => Promise<{}>;
}
export const Store = types.model(
  'Store',
  {
    api: types.optional(ApiClient, {}),
    alertStack: types.optional(AlertStack, { stack: {} }),
    auth: types.optional(Authentication, { token: '' }),
    status: types.optional(ConfigurationStatusModel, { isSetup: false })
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
}))
.actions(self => {
  const load = flow(function*() {
    const request = new DTOs.GetStatus();

    debug('attempting to pull configuration status');
    try {
      const result: DTOs.QueryResponse<DTOs.Status> = yield self.api.query(request);

      self.status.isSetup = result.payload.isSetup;
    } catch (error) {
      debug('received http error: ', error);
    }
  });

  return { load };
});
