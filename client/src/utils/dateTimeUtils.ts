/**
 * Calculate date time diff and return it in s/m/h/d, depends on the time diff
 *
 * @param time - time need to compare with
 *
 * @returns time diff
 */
export const calculateTimeDiff = (time: string) => {
    const currentTime = new Date();
    const prevTime = new Date(time);
    const diff = (currentTime.getTime() - prevTime.getTime()) / 1000;

    const sec = Math.abs(diff);
    if (sec < 60) {
        return sec.toFixed(0) + 's';
    }

    const min = sec / 60;
    if (min < 60) {
        return min.toFixed(0) + 'm';
    }

    const hour = min / 60;
    if (hour < 24) {
        return hour.toFixed(0) + 'h';
    }

    const day = hour / 24;
    return day.toFixed(0) + 'd';
};
