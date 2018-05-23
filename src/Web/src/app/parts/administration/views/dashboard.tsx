import * as React from 'react';
import * as V from 'victory';
import { VectorMap } from 'react-jvectormap';
import numeral from 'numeral';
import Dimensions from 'react-dimensions';
import { hot } from 'react-hot-loader';

import 'jvectormap/jquery-jvectormap.css';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';

import { DashboardStoreType, DashboardStoreModel } from '../stores/dashboard';

interface DashboardProps {
  store?: DashboardStoreType;

  containerWidth?: number;
  containerHeight?: number;
}

const styles = (theme: Theme) => ({
  chart: {
    marginBottom: 20
  },
  map: {
    backgroundColor: theme.palette.background.default
  }
});

class DashboardView extends React.Component<DashboardProps & WithStyles<'chart' | 'map'>, {}> {

  public render() {
    const { store, classes, containerWidth } = this.props;

    return (
      <Grid container justify='center'>
        <Grid item xs={8} className={classes.chart}>
        <Typography variant='headline'>Sales</Typography>
          <V.VictoryChart
            domainPadding={20}
            width={containerWidth}
            height={600}
            responsive={false}
            containerComponent={
              <V.VictoryVoronoiContainer
                labels={(d) => `${d.x} - ${numeral(d.y).format('$0,0.00')}`}
              />
            }>
            <V.VictoryArea data={store.chartData} x='label' y='value' />

            <V.VictoryAxis fixLabelOverlap />
            <V.VictoryAxis dependentAxis domain={{ y: [0, 100] }} tickFormat={(x) => numeral(x).format('$0,0 a')} />
          </V.VictoryChart>
        </Grid>
        <Grid item xs={8}>
          <Grid container spacing={24}>
            <Grid item xs={6} className={classes.chart}>
            <Typography variant='title' color='textSecondary'>Week over Week</Typography>
              <V.VictoryChart
                domainPadding={20}
                height={300}
                responsive={false}
                containerComponent={
                  <V.VictoryVoronoiContainer
                    labels={(d) => `${d.x} - ${numeral(d.y).format('$0,0.00')}`}
                  />
                }>
                <V.VictoryBar data={store.weekOverWeekData} x='day' y='value' />

                <V.VictoryAxis style={{tickLabels: { fontSize: 10, textAnchor: 'end', verticalAnchor: 'middle', angle: -45 }}} />
                <V.VictoryAxis dependentAxis domain={{ y: [0, 100] }} tickFormat={(x) => numeral(x).format('$0,0 a')} />
              </V.VictoryChart>
            </Grid>
            <Grid item xs={6}>
              <VectorMap map={'us_aea'}
                        backgroundColor='#fafafa'
                        ref='map'
                        containerStyle={{
                            width: '100%',
                            height: '100%'
                        }}
                        series={{
                          regions: [{
                            values: store.byStateData,
                            scale: ['#C8EEFF', '#0071A4']
                          }]
                        }}
                        onRegionTipShow={(e, el, code) => {
                          el.html(el.html() + ' ( $' + (store.byStateData[code] || 0) + ' )');
                        }}
                        regionStyle={{
                          initial: {
                            fill: 'black',
                          }
                        }}
                        containerClassName={classes.map}
              />
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    );
  }
}

export default hot(module)(withStyles(styles)<DashboardProps>(Dimensions()(DashboardView)));
