import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { JsonServiceClient } from '@servicestack/client';

import { configure } from 'mobx';
import { Provider } from 'mobx-react';
import { createBrowserHistory } from 'history';

import { AppContainer } from 'app/utils/ioc';
import { createStores } from 'app/stores';
import { App } from 'app';

// enable MobX strict mode
configure({ enforceActions: true });


const client = new JsonServiceClient(API_SERVER);
AppContainer.bind(JsonServiceClient).toConstantValue(client);

// prepare MobX stores
const history = createBrowserHistory();
const rootStore = createStores(history);

// render react DOM
ReactDOM.render(
  <Provider {...rootStore}>
    <App history={history} />
  </Provider>,
  document.getElementById('root')
);
