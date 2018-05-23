import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import { inject, observer } from 'mobx-react';
import * as React from 'react';
import { StoreType } from '../stores';
import Footer from './footer';
import NavBar from './navbar';

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
