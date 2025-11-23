<template>
  <div class="notify" :class="posClass">
    <div v-for="n in items" :key="n.id" class="note" :class="n.level">
      <div class="note-body">
        <div class="note-title">{{ n.title }}</div>
        <div class="note-text">{{ n.text }}</div>
      </div>
      <button class="note-close" @click="$emit('close', n.id)">×</button>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
const props = defineProps({ items: { type: Array, default: () => [] }, position: { type: String, default: 'top-right' } })
const posClass = computed(() => props.position)
</script>

<style scoped>
.notify { position: fixed; inset: 0 auto auto 0; pointer-events: none; z-index: 1000; padding: 12px; }
.notify.top-right { inset: 0 0 auto auto }
.notify.top-left { inset: 0 auto auto 0 }
.notify.bottom-right { inset: auto 0 0 auto }
.notify.bottom-left { inset: auto auto 0 0 }
.note { display: flex; align-items: center; gap: 8px; margin-bottom: 8px; border: 1px solid var(--glass-border); border-radius: 10px; background: var(--glass-surface); color: var(--color-text); box-shadow: 0 10px 30px rgba(0,0,0,0.14); pointer-events: auto; backdrop-filter: blur(var(--glass-blur)); padding: 10px 12px; }
.note .note-title { font-size: 13px; font-weight: 600 }
.note .note-text { font-size: 12px; opacity: 0.9 }
.note .note-close { background: transparent; border: none; color: var(--color-text); font-size: 18px; line-height: 1; cursor: pointer }
.note.info { border-color: #3b82f6 }
.note.ok { border-color: #10b981 }
.note.warn { border-color: #f59e0b }
.note.error { border-color: #ef4444 }
</style>