import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Card, { CardActions, CardContent } from 'material-ui/Card';
import Button from 'material-ui/Button';
import Paper from 'material-ui/Paper';
import Typography from 'material-ui/Typography';

import { Context } from '../../../context';
import { MeStore } from '../stores/me';

import input from '../../../components/inputs/input';
import alertAjax from '../../../components/alertAjax';
import page from '../../../components/page';

interface MeProps {
  store: MeStore;
}

export default function Me(context: Context) {

  const styles = (theme: Theme) => ({
    textField: {
      marginLeft: theme.spacing.unit,
      marginRight: theme.spacing.unit,
      width: 200,
    }
  });

  const Page = page();

  const Input = input(context);
  const AlertAjax = alertAjax(context);

  return withStyles(styles)(observer(class extends React.Component<MeProps & WithStyles<'container' | 'textField'>, {}> {
    public render() {
      const { store, classes } = this.props;

      return (
      <Page className='profile-page'>
        <Card>
          <CardContent>
            <form onSubmit={e => e.preventDefault()}>
              <Typography variant='title'>My Profile</Typography>

              <Input id='firstname' required label='First Name' error={store.validation.first_name} value={store.firstName} onChange={val => store.firstName = val} autoComplete='given-name' />
              <Input id='lastname' required label='Last Name' error={store.validation.last_name} value={store.lastName} onChange={val => store.lastName = val} autoComplete='family-name' />

              <Input id='phone' label='Phone' error={store.validation.phone} value={store.phone} onChange={val => store.phone = val} autoComplete='tel-national' />

            </form>
          </CardContent>
          <CardActions>
            <Button variant='raised' color='primary' onClick={() => store.update()}>
              Submit
            </Button>
          </CardActions>
        </Card>
      </Page>
      );
    }
  }));
}
