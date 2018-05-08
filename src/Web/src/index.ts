
import Debug from 'debug';
import { configure } from 'mobx';

import install from './app/utils/validators';
import { config } from './app/config';
import Log from './app/utils/log';

Log(config.debug.log);
install();

import { App } from './app';
// enable MobX strict mode
configure({ enforceActions: true });

const debug = new Debug('app.entry');

function hideLoading() {
  const loadingEl = document.getElementById('loading');
  loadingEl.classList.add('m-fadeOut');
}

async function run() {
  try {

    const app = new App();
    await app.start();
    hideLoading();
    app.render();
  } catch (e) {
    debug('Error in app:', e);
    throw e;
  }
}

run();
