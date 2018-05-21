import * as React from 'react';
import { inject, observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';

import asyncView from './asyncView';

import NavBar from './navbar';
import Footer from './footer';

import { StoreType } from '../stores';

interface AppViewProps {
  title: string;
  version: string;

  store?: StoreType;
}

const styles = (theme: Theme) => ({
  appRoot: {
    display: 'flex',
    minHeight: '100vh'
  },
  appView: {
    flex: '1 1 auto',
    display: 'flex',
    flexDirection: 'column',
    overflow: 'auto',
  },
  mainView: {
  'display': 'flex',
  'flex': '1',
  'justifyContent': 'center',
  'alignItems': 'flex-start',
  '@media(max-width: 600px)': {
    margin: 10
  }
  }
});

@inject('store')
@observer
class AppView extends React.Component<AppViewProps & WithStyles<'appRoot' | 'appView' | 'mainView'>, {}> {

  public render() {
    const { classes, store, children, title, version } = this.props;

    return (
      <div className={classes.appRoot}>
        <div className={classes.appView}>
          <NavBar title={title} authenticated={store.authenticated} name={store.auth.name} />
          <main className={classes.mainView}>
            {children}
          </main>
          <Footer title={title} version={version} />
        </div>
      </div>
    );

  }
}

export default withStyles(styles as any)<AppViewProps>(AppView);
