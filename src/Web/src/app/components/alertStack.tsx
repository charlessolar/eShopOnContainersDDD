import * as React from 'react';
import { observable, action } from 'mobx';
import { observer } from 'mobx-react';
import Debug from 'debug';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Snackbar from 'material-ui/Snackbar';
import IconButton from 'material-ui/IconButton';
import CloseIcon from 'material-ui-icons/Close';

import Alert from './alert';

import { Context } from '../context';

const debug = new Debug('alerts');

export interface AlertStack {
  View: React.ComponentType;
  add(type: 'info' | 'warn' | 'error', message: string);
  remove(id: string);
}
interface Message {
  id: string;
  type: 'info' | 'warn' | 'error';
  message: string;
}

export default function(context: Context, { limit = 10 }): AlertStack {

  const style = (theme: Theme) => ({
    close: {
      width: theme.spacing.unit * 4,
      height: theme.spacing.unit * 4,
    }
  });

  class Store {
    constructor(limit: number) {
      this.messages = [];
    }

    @observable
    public messages: Message[];

    @action
    public add(type: 'info' | 'warn' | 'error', message: string) {
      debug('add %s: %s', type, message);
      const item: Message = {
        id: Math.random().toString(10).split('.')[1],
        type,
        message
      };
      if (this.messages.length >= limit) {
        this.remove(this.messages[0].id);
      }

      this.messages.push(item);
    }
    @action
    public remove(id: string) {
      debug('remove %s', id);
      this.messages.findIndex((message, idx) => {
        if (message.id === id) {
          this.messages.splice(idx, 1);
          return true;
        }
        return false;
      });
    }
  }

  const store = new Store(limit);

  class AlertComponent extends React.Component<{} & WithStyles<'close'>, {}> {

    @action
    private handleClose(id: string) {
      store.remove(id);
    }

    public render() {
      const { classes } = this.props;

      return (
        <div>
          {store.messages.map((message, key) => (
            <Snackbar
              key={key}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={true}
              autoHideDuration={6000}
              >
              <Alert key={key} type={message.type} message={message.message} action={
                <IconButton
                  key='close'
                  aria-label='Close'
                  color='inherit'
                  className={classes.close}
                  onClick={(e) => this.handleClose(message.id)}
                >
                  <CloseIcon />
                </IconButton>
              }/>
            </Snackbar>
          ))}
        </div>
      );
    }
  }

  const view = withStyles(style)(observer(AlertComponent));

  return {
    View: view,
    add(type: 'info' | 'warn' | 'error', message: string) {
      store.add(type, message);
    },
    remove(id: string) {
      store.remove(id);
    }
  };
}
