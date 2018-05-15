import * as React from 'react';
import glamorous from 'glamorous';
import { observable, action } from 'mobx';
import { inject, observer } from 'mobx-react';

import { withStyles, WithStyles } from 'material-ui/styles';
import AppBar from 'material-ui/AppBar';
import Button from 'material-ui/Button';
import Toolbar from 'material-ui/Toolbar';
import Typography from 'material-ui/Typography';

import Menu, { Route } from './menu';
import { StoreType } from '../stores';

interface NavBarProps {
  authenticated: boolean;
  name: string;
  title: string;

  store?: StoreType;
}

class Store {
  constructor(private _store: StoreType) { }

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
      this._store.history.push(menuItem);
    } else {
      this._store.history.push(menuItem.route);
    }
    this.close();
  }
}

const styles = theme => ({
  root: {
  },
  title: {
    flex: 1,
    cursor: 'pointer'
  }
});

@inject('store')
class NavBar extends React.Component<NavBarProps & WithStyles<'root' | 'title'>, {}> {
  private _store: Store;
  constructor(props: any) {
    super(props);
    this._store = new Store(props.store);
  }

  public render() {
    const { title, store, authenticated, name, classes } = this.props;
    return (
      <header className={classes.root}>
        <AppBar position='static'>
          <Toolbar>
            <Typography onClick={() => this._store.navChange('/')} variant='title' color='inherit' className={classes.title}>{title}</Typography>
            {authenticated ? (
              <>
                {store.auth.admin && (
                  <>
                    <Button color='inherit' onClick={() => this._store.navChange('/campaigns')}>My Campaigns</Button>
                    <Button color='inherit' onClick={() => this._store.navChange('/administrate')}>Administrate</Button>
                  </>
                )}

                <Button color='inherit' onClick={() => this._store.navChange('/orders')}>My Orders</Button>
                <Menu authenticated={authenticated} name={name} navChange={item => this._store.navChange(item)} />
              </>
            )
              :
              <Button color='inherit' onClick={() => this._store.navChange('/login')}>Login</Button>
            }
          </Toolbar>
        </AppBar>

      </header>
    );
  }

}

export default withStyles(styles)<NavBarProps>(NavBar);
