import * as React from 'react';
import { inject } from 'mobx-react';
import { History } from 'history';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';

import { StoreType } from '../../../stores';

interface ToolbarProps {
  store?: StoreType;
}

const styles = (theme: Theme) => ({
  appView: {
    flex: '1 1 auto',
    display: 'flex',
    flexDirection: 'column',
    overflow: 'auto',
    color: theme.palette.primary.main
  },
  mainView: {
    'display': 'flex',
    'flex': '1',
    'justifyContent': 'center',
    'alignItems': 'flex-start',
    'marginTop': 2,
    '@media(max-width: 600px)': {
      margin: 10
    }
  },
  appBar: {
    backgroundColor: theme.palette.background.default,
    color: theme.palette.common.black
  }
});

@inject('store')
class AdministrateToolbarView extends React.Component<ToolbarProps & WithStyles<'appView' | 'mainView' | 'appBar'>, {}> {
  private handleChange = (e: any, path: string) => {
    const { store } = this.props;
    store.history.push(path);
  }

  public render() {
    const { classes, store, children } = this.props;

    const pathname = store.history.location.pathname;

    return (
      <div className={classes.appView}>
        <AppBar position='static' className={classes.appBar}>
            <Tabs value={pathname} onChange={this.handleChange} indicatorColor='primary'>
              <Tab value='/administrate' label='Dashboard'/>
              <Tab value='/administrate/catalog' label='Catalog' />
              <Tab value='/administrate/orders' label='Orders' />
              <Tab value='/administrate/buyers' label='Buyers' />
            </Tabs>
        </AppBar>
        <div className={classes.mainView}>
          {children}
        </div>
      </div>
    );
  }
}

export default withStyles(styles as any)<ToolbarProps>(AdministrateToolbarView);
