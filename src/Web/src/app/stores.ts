import { IType, types, getEnv, getRoot, flow, applySnapshot, onSnapshot, addDisposer } from 'mobx-state-tree';
import { JsonServiceClient } from '@servicestack/client';
import { History } from 'history';
import Debug from 'debug';

import { Theme } from 'material-ui/styles';
import { DTOs } from './utils/eShop.dtos';
import theme from './theme';

const debug = new Debug('stores');

interface JWTPayload {
  // issuer
  iss: string;
  // subject
  sub: string;
  // issued at
  iat: number;
  // expiration time
  exp: number;
  // audience
  aud?: string;
}

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
  roles: string[];

  readonly admin: boolean;
  updateToken(token: string): void;
  reset(): Promise<{}>;
}
const Authentication = types.model(
  'Authentication',
  {
    email: types.maybe(types.string),
    expires: types.maybe(types.number),
    token: types.optional(types.string, ''),
    roles: types.optional(types.array(types.string), [])
  })
  .views(self => ({
    get authenticated() {
      return self.token && self.token !== ''; // && expires > now
    },
    get admin() {
      return self.roles.indexOf('administrator') !== -1;
    }
  }))
  .actions(self => {
    const updateToken = (token: string) => {

      try {
        const decoded = token.split('.')[1].replace('-', '+').replace('_', '/');
        const profile = JSON.parse(window.atob(decoded)) as JWTPayload;

        debug('decoded token: ', profile);

        self.token = token;
        self.expires = profile.exp;

        const client = getEnv(self).client as JsonServiceClient;
        client.setBearerToken(self.token);
      } catch (error) {
        throw new Error('failed to decode JWT token');
      }
    };
    const reset = flow(function*() {
      const client = getEnv(self).client as JsonServiceClient;

      const request = new DTOs.Authenticate();
      request.provider = 'logout';
      try {
      yield client.post(request);

      self.token = '';
      self.email = '';

      self.expires = 0;

      client.setBearerToken('');
      } catch (error) {
        debug('failed to logout', error);
      }
    });
    const afterCreate = () => {
      const authStorage = localStorage.getItem('auth.eShop');
      applySnapshot(self, authStorage ? JSON.parse(authStorage) : {});

      const disposer = onSnapshot(self, state => {
        localStorage.setItem('auth.eShop', JSON.stringify(state));
      });
      addDisposer(self, disposer);
    };
    return { updateToken, reset, afterCreate };
  });

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
  client: JsonServiceClient;

  readonly authenticated: boolean;
  load: () => Promise<{}>;
}
export const Store = types.model(
  'Store',
  {
    api: types.optional(ApiClient, {}),
    alertStack: types.optional(AlertStack, { stack: {} }),
    auth: types.optional(Authentication, {}),
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
  },
  get client() {
    return getEnv(self).client;
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
