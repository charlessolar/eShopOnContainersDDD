import { Container } from "inversify";
import { makeFluentProvideDecorator } from "inversify-binding-decorators";
import "reflect-metadata";

import getDecorators from 'inversify-inject-decorators';

export const AppContainer = new Container();
const provide = makeFluentProvideDecorator(AppContainer);

export const provideSingleton = function(identifier) {
	return provide(identifier)
		      .inSingletonScope()
		      .done();
};

export const inject = getDecorators(AppContainer).lazyInject;
