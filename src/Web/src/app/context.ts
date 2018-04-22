import { Theme } from 'material-ui/styles';
import { History } from 'history';
import createBrowserHistory from 'history/createBrowserHistory';
import theme from './theme';

import { config, Config } from './config';
import { AuthModule } from './parts/auth/authModule';
import { AdminModule } from './parts/admin/adminModule';

import AlertStackCreation, { AlertStack } from './components/alertStack';

interface Parts {
  auth: AuthModule;
  admin: AdminModule;
}

export class Context {
  public theme: Theme;
  public history: History;
  public config: Config;
  public parts: Parts;
  public alertStack: AlertStack;

  constructor() {
    this.theme = theme();
    this.history = createBrowserHistory();
    this.config = config;
    this.alertStack = AlertStackCreation(this, { limit: 3 });

    this.parts = {
      auth: new AuthModule(this),
      admin: new AdminModule(this),
    };
  }
}
