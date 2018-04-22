import * as React from 'react';
import { render } from 'react-dom';
import { parse } from 'qs';
import Debug from 'debug';

import { MuiThemeProvider } from 'material-ui/styles';

import { createRouter } from './router';
import { Context } from './context';
import asyncView from './components/asyncView';
import applicationView from './components/applicationView';
import alertStack from './components/alertStack';

const debug = new Debug('client');

export class Client {
  constructor(private _context: Context) {
    _context.history.listen((loc) => this.onLocationChange(loc as any));
    this.onLocationChange(_context.history.location as any);
  }

  public onRenderComplete(route: any, location: UniversalRouterContext) {
    document.title = `${route.title} - ${this._context.config.title}`;
  }

  public async onLocationChange(location: UniversalRouterContext) {
    debug('onLocationChange', location);

    let component: any;
    let route: any;
    try {
      route = await createRouter(this._context).resolve({
        pathname: location.pathname
      });
      component = route.component;
    } catch (error) {
      debug('routing exception', error);
      if (error.status === 404) {
        component = React.createElement(asyncView(this._context), {
          getComponent: () => import('./components/notFound')
        });
        route = { title: 'Page not found' };
      }
    }

    if (component) {
      const { theme } = this._context;

      const authStores = this._context.parts.auth.stores;
      const Layout = applicationView(this._context);
      const AlertStack = this._context.alertStack.View;

      const layout = (
        <MuiThemeProvider theme={theme}>
          <Layout authenticated={authStores.auth.authenticated} email={authStores.me.email}>
            {component}
            <AlertStack/>
          </Layout>
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
