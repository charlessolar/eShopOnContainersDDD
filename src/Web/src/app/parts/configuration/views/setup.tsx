import Button from '@material-ui/core/Button';
import CircularProgress from '@material-ui/core/CircularProgress';
import Fade from '@material-ui/core/Fade';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import { WithStyles, withStyles } from '@material-ui/core/styles';
import { observer } from 'mobx-react';
import * as React from 'react';
import { inject_factory } from '../../../utils';
import { SetupModel, SetupType } from '../stores/setup';

interface SetupProps {
  store?: SetupType;
}

const styles = () => ({
  root: {
    flexGrow: 1
  },
  page: {
    height: '50vh'
  }
});

@inject_factory(store => SetupModel.create({}, { store }))
@observer
class SetupView extends React.Component<SetupProps & WithStyles<'root' | 'page'>, {}> {

  private submitSeed = () => {
    const { store } = this.props;

    store.seed();
  }

  public render() {
    const { store, classes } = this.props;

    return (
      <Grid container className={classes.root}>
        <Grid item xs={12}>
          <Grid container justify='center' alignItems='center' className={classes.page}>
            <Grid item xs={3}>
              {store.loading ?
                <Fade in={true} unmountOnExit>
                  <>
                    <Typography variant='title'>Seeding data....</Typography>
                    <CircularProgress />
                  </>
                </Fade> :
                <Fade in={true} unmountOnExit>
                  <>
                    <Typography variant='title'>Welcome to eShopOnContainers!</Typography>
                    <Typography variant='subheading' paragraph gutterBottom>This demo requires data to be seeded into the app before use.  Please use the button below to seed this initial data</Typography>
                    <Button onClick={this.submitSeed} size='large' color='primary' variant='raised'>Seed Data</Button>
                  </>
                </Fade>
              }
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    );
  }
}

export default withStyles(styles)(SetupView);
