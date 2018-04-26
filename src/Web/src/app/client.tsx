import * as React from 'react';
import { render } from 'react-dom';
import { Provider } from 'mobx-react';

import { parse } from 'qs';
import Debug from 'debug';

import { MuiThemeProvider } from 'material-ui/styles';

import theme from './theme';
import { createRouter } from './router';
import { StoreType } from './stores';
import { Modules } from './modules';
import { config } from './config';

import asyncView from './components/asyncView';
import applicationView from './components/applicationView';
import AlertStack from './components/alertStack';

const debug = new Debug('client');

export class Client {
  constructor(private _store: StoreType, private _modules: Modules) {
    _store.history.history.listen((loc) => this.onLocationChange(loc as any));
    this.onLocationChange(_store.history.history.location as any);
  }

  public onRenderComplete(route: any, location: UniversalRouterContext) {
    document.title = `${route.title} - ${config.title}`;
  }

  public async onLocationChange(location: UniversalRouterContext) {
    debug('onLocationChange', location);

    let component: any;
    let route: any;
    try {
      route = await createRouter(this._store, this._modules).resolve({
        pathname: location.pathname
      });
      component = route.component;
    } catch (error) {
      debug('routing exception', error);
      if (error.status === 404) {
        component = React.createElement(asyncView, {
          getComponent: () => import('./components/notFound')
        });
        route = { title: 'Page not found' };
      }
    }

    if (component) {

      const Layout = applicationView();

      const layout = (
        <MuiThemeProvider theme={theme()}>
          <Provider store={this._store}>
            <Layout authenticated={true} email={''} title={config.title} version={config.build.version}>
              {component}
              <AlertStack/>
            </Layout>
          </Provider>
        </MuiThemeProvider>
      );
      render(
        layout,
        document.getElementById('application'),
        () => this.onRenderComplete(route, location)
      );

    }
  }
}
