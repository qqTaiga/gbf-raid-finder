import devSettings from 'app-settings-dev.json';
import prodSettings from 'app-settings-prod.json';

/**
 * Get core api domain url.
 *
 *
 * @returns Core api domain url
 **/
const getDomain = (): string => {
    let settings;
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        settings = devSettings;
    } else {
        settings = prodSettings;
    }
    return settings.api.domain;
};

/**
 * Urls utils, use to get api url.
 **/
export const urls = {
    /**
     * Api
     **/
    api: {
        /**
         * Retrive boss list
         **/
        getBossList: (): string => {
            return getDomain() + '/gbf/list';
        },
    },
    /**
     * Web socket url
     */
    websocket: {
        /**
         * Connect raids
         */
        raids: (): string => {
            return getDomain() + '/raids';
        },
    },
};
