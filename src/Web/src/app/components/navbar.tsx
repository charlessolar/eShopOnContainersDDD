import * as React from 'react';
import glamorous from 'glamorous';
import { observable, action } from 'mobx';

import { withStyles, WithStyles } from 'material-ui/styles';
import AppBar from 'material-ui/AppBar';
import Button from 'material-ui/Button';
import Toolbar from 'material-ui/Toolbar';
import Typography from 'material-ui/Typography';

import MenuComponent, { Route } from './menu';
import { Context } from '../context';

interface NavBarProps {
  authenticated: boolean;
  email: string;
  title: string;
}

class Store {
  constructor(private _context: Context) {
  }

  @observable
  public open: boolean;

  public toggle() {
    this.open = !this.open;
  }
  public close() {
    this.open = false;
  }
  @action
  public navChange(menuItem: Route | string) {
    if (typeof menuItem === 'string') {
      this._context.history.push(menuItem);
    } else {
      this._context.history.push(menuItem.route);
    }
    this.close();
  }
}

export default function NavBar(context: Context) {
  const { theme, config } = context;
  const Menu = MenuComponent(context);

  const store = new Store(context);

  const styles = {
    root: {
      flexGrow: 1,
    },
    title: {
      flex: 1,
      cursor: 'pointer'
    }
  };

  return withStyles(styles)(class extends React.Component<NavBarProps & WithStyles<'root' | 'title'>, {}> {

    public render() {
      const { title, authenticated, email, classes } = this.props;
      return (
        <header>
          <AppBar position='static'>
            <Toolbar>
              <Typography onClick={() => store.navChange('/')} variant='title' color='inherit' className={classes.title}>{title}</Typography>
              {authenticated && (<Menu authenticated={authenticated} email={email} navChange={item => store.navChange(item)}/>)}
            </Toolbar>
          </AppBar>

        </header>
      );
    }

  });
}
