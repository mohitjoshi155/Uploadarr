import { NuxtApp } from '@nuxt/types/app';
import { Module, VuexModule } from 'vuex-module-decorators';
import VueI18n, { IVueI18n } from 'vue-i18n';

@Module({
	name: 'globalStore',
	namespaced: true,
	stateFactory: true,
})
export default class GlobalStore extends VuexModule {
	debugMode = true;
	get Nuxt(): NuxtApp {
		return window.$nuxt;
	}

	get i18n(): VueI18n & IVueI18n {
		return window.$nuxt.$i18n;
	}

	get isDebugMode(): boolean {
		return this.debugMode;
	}
}
